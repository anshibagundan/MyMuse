import { useState } from 'react';
import { useAuth } from '@/hooks/useAuth';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import { api } from '@/lib/api';

interface TagCreateFormProps {
    onSuccess?: () => void;
    existingTagsCount: number;
}

export function TagCreateForm({ onSuccess, existingTagsCount }: TagCreateFormProps) {
    const [isOpen, setIsOpen] = useState(false);
    const [name, setName] = useState('');
    const [roomType, setRoomType] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const { user } = useAuth();
    const { toast } = useToast();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!user) return;

        try {
            setIsSubmitting(true);
            await api.createTag({
                name,
                roomType,
                user_id: user.ID,
                order: existingTagsCount + 1
            });

            toast({
                title: "成功",
                description: "新しい部屋を作成しました。",
            });

            setName('');
            setIsOpen(false);
            onSuccess?.();
        } catch (error) {
            toast({
                title: "エラー",
                description: "部屋の作成に失敗しました。",
                variant: "destructive"
            });
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild>
                <Button variant="outline" className="w-full">
                    部屋を作る
                </Button>
            </DialogTrigger>
            <DialogContent className="bg-white">
                <DialogHeader>
                    <DialogTitle>新しい部屋を作る</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label htmlFor="name" className="block text-sm font-medium text-gray-700">
                            部屋の名前
                        </label>
                        <Input
                            id="name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            placeholder="部屋の名前を入力"
                            required
                            className="mt-1"
                        />
                    </div>
                    <div>
                        <label htmlFor="roomType" className="block text-sm font-medium text-gray-700">
                            部屋の種類
                        </label>
                        <select
                            id="roomType"
                            value={roomType}
                            onChange={(e) => setRoomType(e.target.value)}
                            required
                            className="mt-1 block w-full p-2 border border-gray-300 rounded"
                        >
                            <option value="">選択してください</option>
                            <option value="廊下">廊下</option>
                            <option value="通常部屋">通常部屋</option>
                            <option value="春の部屋">春の部屋</option>
                            <option value="夏の部屋">夏の部屋</option>
                            <option value="秋の部屋">秋の部屋</option>
                            <option value="冬の部屋">冬の部屋</option>
                        </select>
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
                            {isSubmitting ? '作成中...' : '作成'}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
}