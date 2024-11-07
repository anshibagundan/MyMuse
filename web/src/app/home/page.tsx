'use client';

import { useRouter } from 'next/navigation';
import { usePhotoGallery } from '@/hooks/usePhotoGallery';
import { useAuth } from '@/hooks/useAuth';
import { TagCreateForm } from '@/components/tag/TagCreateForm';
import {TagContainer} from "@/components/home/TagContainer";
import {useEffect, useRef, useState} from "react";
import Sortable from 'sortablejs';

// 参照を保持するオブジェクトの型を定義
interface PhotoContainerRefs {
    [key: string]: HTMLDivElement | null;
}

interface SortableInstances {
    [key: string]: Sortable | null;
}

export default function HomePage() {
    const router = useRouter();
    const { user, setUser} = useAuth();
    const { data, isLoading: galleryLoading, error, mutate } = usePhotoGallery(user?.ID || "");

    useEffect(() => {
        if (!user) {
            router.push('/login');
            console.log("userが見つかりません。ログインしてください")
        }
        setUser(user)
    }, [router]);

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
    const toggleSortingMode = (userId: string) => {
        setEditingTags(prev => {
            const newState = { ...prev, [userId]: !prev[userId] };

            if (newState[userId]) {
                // 並び替えモードをONにする
                const container = photoContainerRefs.current[userId];
                if (container && !sortableInstancesRef.current[userId]) {
                    sortableInstancesRef.current[userId] = new Sortable(container, {
                        animation: 150,
                        ghostClass: 'sortable-ghost',
                        chosenClass: 'sortable-chosen',
                        dragClass: 'sortable-drag'
                    });
                }
            } else {
                // 並び替えモードをOFFにする
                if (sortableInstancesRef.current[userId]) {
                    sortableInstancesRef.current[userId]?.destroy();
                    sortableInstancesRef.current[userId] = null;
                }
            }

            return newState;
        });
    };

    // 並び替えをキャンセル
    const cancelSorting = (userId: string) => {
        if (sortableInstancesRef.current[userId]) {
            sortableInstancesRef.current[userId]?.destroy();
            sortableInstancesRef.current[userId] = null;
        }
        setEditingTags(prev => ({ ...prev, [userId]: false }));
        // データを再取得して並び順をリセット
        mutate(userId);
    };

    // 並び替えを保存
    const saveSorting = async (userId: string) => {
        const container = photoContainerRefs.current[userId];
        if (!container) return;

        const photoElements = container.children;
        const orderData = Array.from(photoElements).map((el, index) => ({
            id: Number((el as HTMLElement).getAttribute('data-tag-id')),
            order: index + 1
        }));

        try {
            const response = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/web/tag/order/${userId}`, {
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
            toggleSortingMode(userId);
            // データを再取得
            mutate(userId);
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


    if (!user) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="text-lg">Please log in to view the gallery.</div>
            </div>
        );
    }

    if (galleryLoading) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="text-lg">Loading photos...</div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="text-red-500">Error: {error.message}</div>
            </div>
        );
    }

    return (
        <div
            className="grid grid-rows-[20px_minmax(calc(100vh-160px),1fr)_20px_minmax(calc(100vh-180px),1fr)]
  items-center justify-items-center min-h-screen p-8 pb-20 gap-8 sm:p-20"
            style={{fontFamily: 'var(--font-geist-sans)'}}
        >
            <main className="flex flex-col gap-8 row-start-2 items-center sm:items-start">
                <div className="">
                    <p className="custom-paragraph">
                        バーチャルで
                    </p>
                    <br/>
                    <br/>
                    <p className="custom-paragraph2">
                        あなただけの
                        <br/>
                        <br/>
                        美術館を作ろう
                    </p>
                </div>
            </main>
            <div className="row-start-4 border rounded-lg p-4 shadow bg-white overflow-y-auto "
                 style={{width: 'calc(100vw - 160px)', minHeight: 'calc(100vh - 240px)'}}>
                {data?.tags === null ? (
                        <p className="text-gray-500">まだ部屋がありません。</p>
                ) : (
                    <div>
                        {!editingTags[user.ID] ? (
                            <button
                                onClick={() => toggleSortingMode(user.ID)}
                                className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
                            >
                                順番を変更
                            </button>
                        ) : (
                            <div className="flex gap-2">
                                <button
                                    onClick={() => saveSorting(user.ID)}
                                    className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition-colors"
                                >
                                    保存
                                </button>
                                <button
                                    onClick={() => cancelSorting(user.ID)}
                                    className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600 transition-colors"
                                >
                                    キャンセル
                                </button>
                            </div>
                        )}
                    <div ref={setPhotoContainerRef(user.ID)}
                    >
                        {data?.tags.map((tag) => (
                        <TagContainer
                            key={tag.ID}
                            data-tag-id={tag.ID}  // data-tag-id属性を追加
                            tag={tag}
                            onSortSuccess={() => mutate(user.ID)}
                            onDeleteSuccess={() => mutate(user.ID)}
                            onPhotoCreateSuccess={() => mutate(user.ID)}
                        />
                        ))}
                    </div>
                    </div>
                    )}
                <TagCreateForm existingTagsCount={data?.tags?.length ?? 0} onSuccess={() => mutate(user.ID)} />
                </div>
            </div>
        );
}