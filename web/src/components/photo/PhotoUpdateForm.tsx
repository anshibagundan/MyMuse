import { useState, useEffect, useRef } from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import { PencilSquareIcon } from '@heroicons/react/24/solid'; // Heroiconsからインポート
import { api } from '@/lib/api';

interface PhotoEditProps {
    photoId: number;  // 編集する写真のID
    currentTitle: string;  // 現在のタイトル
    currentDetailedTitle: string;  // 現在の詳細タイトル
    onSuccess?: () => void;  // 編集成功時のコールバック
}

export function PhotoEdit({ photoId, currentTitle, currentDetailedTitle, onSuccess }: PhotoEditProps) {
    const [isOpen, setIsOpen] = useState(false);
    const [title, setTitle] = useState(currentTitle);
    const [detailedTitle, setDetailedTitle] = useState(currentDetailedTitle);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const { toast } = useToast();

    useEffect(() => {
        // ダイアログが開かれたときに、タイトルを初期化
        setTitle(currentTitle);
        setDetailedTitle(currentDetailedTitle);
    }, [currentTitle, currentDetailedTitle, isOpen]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            setIsSubmitting(true);
            await api.updatePhoto({
                id: photoId,
                title: title,
                detailed_title: detailedTitle
            });  // ここでAPI呼び出し

            toast({
                title: "成功",
                description: "写真が更新されました。",
            });

            setIsOpen(false);
            onSuccess?.();  // コールバック
        } catch (error) {
            toast({
                title: "エラー",
                description: "写真の更新に失敗しました。",
                variant: "destructive",
            });
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild>
                <Button variant="outline" className="flex items-center">
                    <PencilSquareIcon className="w-5 h-5 text-blue-500 mr-2" />
                    編集
                </Button>
            </DialogTrigger>
            <DialogContent className="bg-white">
                <DialogHeader>
                    <DialogTitle>写真を編集</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label htmlFor="title" className="block text-sm font-medium text-gray-700">
                            タイトル
                        </label>
                        <Input
                            id="title"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            placeholder="タイトルを入力"
                            required
                            className="mt-1"
                        />
                    </div>
                    <div>
                        <label htmlFor="detailedTitle" className="block text-sm font-medium text-gray-700">
                            詳細タイトル
                        </label>
                        <Input
                            id="detailedTitle"
                            value={detailedTitle}
                            onChange={(e) => setDetailedTitle(e.target.value)}
                            placeholder="詳細タイトルを入力（任意）"
                            className="mt-1"
                        />
                    </div>
                    <div className="flex justify-end gap-2">
                        <Button
                            type="button"
                            variant="outline"
                            onClick={() => setIsOpen(false)}
                        >
                            キャンセル
                        </Button>
                        <Button
                            type="submit"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? '更新中...' : '保存'}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
}
