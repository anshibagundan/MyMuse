import { useState } from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import { TrashIcon } from "@heroicons/react/24/solid"; // 正しいインポート方法

import { api } from '@/lib/api';

interface DeleteConfirmationDialogProps {
    photoId: number;
    onDeleteSuccess: () => void;
}

export function PhotoDeleteForm({ photoId, onDeleteSuccess }: DeleteConfirmationDialogProps) {
    const [isOpen, setIsOpen] = useState(false);
    const [isDeleting, setIsDeleting] = useState(false);
    const { toast } = useToast();

    const handleDelete = async () => {
        setIsDeleting(true);
        try {
            await api.deletePhoto(photoId.toString());
            toast({
                title: "削除成功",
                description: "写真が削除されました。",
            });
            onDeleteSuccess();
            setIsOpen(false);
        } catch (error) {
            toast({
                title: "削除エラー",
                description: "写真の削除に失敗しました。",
                variant: "destructive",
            });
        } finally {
            setIsDeleting(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild>
                <Button variant="outline" className="flex items-center">
                    <TrashIcon className="w-5 h-5 text-red-500 mr-2" />
                    削除
                </Button>
            </DialogTrigger>
            <DialogContent className="bg-white">
                <DialogHeader>
                    <DialogTitle>本当に削除しますか？</DialogTitle>
                </DialogHeader>
                <div className="space-y-4">
                    <p className="text-sm text-gray-600">この操作は取り消せません。</p>
                    <div className="flex justify-end gap-2">
                        <Button
                            type="button"
                            variant="outline"
                            onClick={() => setIsOpen(false)}
                        >
                            キャンセル
                        </Button>
                        <Button
                            type="button"
                            variant="destructive"
                            onClick={handleDelete}
                            disabled={isDeleting}
                        >
                            {isDeleting ? '削除中...' : '削除'}
                        </Button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    );
}
