import { TagEditForm } from '@/components/tag/TagUpdateForm';
import { TagDeleteForm } from '@/components/tag/TagDeleteForm';
import { PhotoCreateForm } from '@/components/photo/PhotoCreateForm';
import { PhotoCard } from '@/components/home/PhotoCard';
import Sortable from 'sortablejs';
import {useEffect, useRef, useState} from 'react';
import {PhotoTag} from "@/types/photoGallery";

interface TagContainerInterface {
    tag: PhotoTag
    onSortSuccess: () => void;
    onDeleteSuccess: () => void;
    onPhotoCreateSuccess: () => void;
}

// 参照を保持するオブジェクトの型を定義
interface PhotoContainerRefs {
    [key: string]: HTMLDivElement | null;
}

interface SortableInstances {
    [key: string]: Sortable | null;
}

export const TagContainer = ({ tag, onSortSuccess, onDeleteSuccess, onPhotoCreateSuccess }: TagContainerInterface) => {

    // 編集モードの状態管理
    const [editingTags, setEditingTags] = useState<{ [key: string]: boolean }>({});

    // Sortableインスタンスの参照を保持
    const sortableInstancesRef = useRef<SortableInstances>({});
    const photoContainerRefs = useRef<PhotoContainerRefs>({});

    const setPhotoContainerRef = (tagId: string) => (el: HTMLDivElement | null) => {
        if (photoContainerRefs.current) {
            photoContainerRefs.current[tagId] = el;
        }
    };

    // 並び替えモードの切り替え
    const toggleSortingMode = (tagId: string) => {
        setEditingTags(prev => {
            const newState = { ...prev, [tagId]: !prev[tagId] };

            if (newState[tagId]) {
                // 並び替えモードをONにする
                const container = photoContainerRefs.current[tagId];
                if (container && !sortableInstancesRef.current[tagId]) {
                    sortableInstancesRef.current[tagId] = new Sortable(container, {
                        animation: 150,
                        ghostClass: 'sortable-ghost',
                        chosenClass: 'sortable-chosen',
                        dragClass: 'sortable-drag'
                    });
                }
            } else {
                // 並び替えモードをOFFにする
                if (sortableInstancesRef.current[tagId]) {
                    sortableInstancesRef.current[tagId]?.destroy();
                    sortableInstancesRef.current[tagId] = null;
                }
            }

            return newState;
        });
    };

    // 並び替えをキャンセル
    const cancelSorting = (tagId: string) => {
        if (sortableInstancesRef.current[tagId]) {
            sortableInstancesRef.current[tagId]?.destroy();
            sortableInstancesRef.current[tagId] = null;
        }
        setEditingTags(prev => ({ ...prev, [tagId]: false }));
        // データを再取得して並び順をリセット
        onSortSuccess();
    };

    // 並び替えを保存
    const saveSorting = async (tagId: string) => {
        const container = photoContainerRefs.current[tagId];
        if (!container) return;

        const photoElements = container.children;
        const orderData = Array.from(photoElements).map((el, index) => ({
            id: Number((el as HTMLElement).dataset.id),
            order: index + 1
        }));

        try {
            const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/web/tag/${tagId}/photos/order`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(orderData),
            });

            if (!response.ok) {
                throw new Error('Failed to save order');
            }

            // 並び替えモードを終了
            toggleSortingMode(tagId);
            // データを再取得
            onSortSuccess();
        } catch (error) {
            console.error('Error saving photo order:', error);
            // エラー処理（必要に応じてユーザーに通知）
        }
    };

    // コンポーネントのクリーンアップ
    useEffect(() => {
        return () => {
            // すべてのSortableインスタンスを破棄
            Object.values(sortableInstancesRef.current).forEach(instance => {
                if (instance) instance.destroy();
            });
        };
    }, []);

    return (
        <div key={`tag_${tag.ID}`} data-tag-id={tag.ID} className="mb-8"
             style={{
                 backgroundImage: `url("${
                     tag.roomType === "春の部屋"
                         ? "/spring.svg"
                         : tag.roomType === "夏の部屋"
                             ? "/summer.svg"
                             : tag.roomType === "秋の部屋"
                                 ? "/autumn.svg"
                                 : tag.roomType === "冬の部屋"
                                     ? "/winter.svg"
                                     : tag.roomType === "廊下"
                                         ? "/hallway.svg"
                                         : "/default.svg" // 通常部屋など、他のタイプやデフォルト
                 }")`,
                 backgroundPosition: 'center',
                 backgroundSize: '1300px 650px', // 固定サイズ
                 backgroundRepeat: 'no-repeat',
                 width: '100%', // 必要に応じて親要素の幅を調整
                 overflow: 'hidden', // はみ出た部分を隠す
             }}
        >
            <div style={{
                padding: '10px',
                display: 'flex',
                gap: '20px',
                backgroundColor: 'rgba(255, 255, 255, 0.8)',
                alignItems: 'center'
            }}
            >
                <div className="text-xl font-semibold">{tag.name}</div>
                <TagEditForm tagId={tag.ID} currentName={tag.name} currentRoomType={tag.roomType} onSuccess={onDeleteSuccess}/>
                <TagDeleteForm tagId={tag.ID} onDeleteSuccess={onDeleteSuccess}
                               disabled={(tag.photos?.length ?? 0) !== 0}/>
                <PhotoCreateForm tagId={tag.ID} existingPhotosCount={tag.photos?.length ?? 0}
                                 onSuccess={onPhotoCreateSuccess}
                                 disabled={(tag.photos?.length ?? 0) >= 9}
                />
                {!editingTags[tag.ID] ? (
                    <button
                        onClick={() => toggleSortingMode(tag.ID)}
                        className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
                    >
                        順番を変更
                    </button>
                ) : (
                    <div className="flex gap-2">
                        <button
                            onClick={() => saveSorting(tag.ID)}
                            className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition-colors"
                        >
                            保存
                        </button>
                        <button
                            onClick={() => cancelSorting(tag.ID)}
                            className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600 transition-colors"
                        >
                            キャンセル
                        </button>
                    </div>
                )}
            </div>
            {tag.photos === null ? (
                <p className="text-gray-500">部屋の中に写真がありません。</p>
            ) : (
                <div ref={setPhotoContainerRef(tag.ID)}
                     className="flex overflow-x-auto gap-4"
                     style={{margin: '10px', padding: '0 10px 10px 10px'}}
                >
                    {tag.photos.map((photo) => (
                        <PhotoCard key={photo.ID} photo={photo} onDeleteSuccess={onDeleteSuccess}/>
                    ))}
                </div>
            )}
        </div>
    );
};