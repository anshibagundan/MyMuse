import { useState, useRef } from 'react';
import { useAuth } from '@/hooks/useAuth';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useToast } from "@/components/ui/use-toast";
import { api } from '@/lib/api';
import { S3Client, PutObjectCommand } from "@aws-sdk/client-s3";


interface PhotoCreateFormProps {
    tagId: string;
    existingPhotosCount: number;
    onSuccess?: () => void;
    disabled?: boolean
}

// S3クライアントを初期化
const s3Client = new S3Client({
    region: process.env.NEXT_PUBLIC_REGION!,
    credentials: {
        accessKeyId: process.env.NEXT_PUBLIC_ACCESS_KEY_ID!,
        secretAccessKey: process.env.NEXT_PUBLIC_SECRET_ACCESS_KEY!,
    },
});

async function uploadToS3(file: File): Promise<string | null> {
    const fileName = `${Date.now()}-${file.name}`;
    const params = {
        Bucket: process.env.NEXT_PUBLIC_S3_BUCKET_NAME!,
        Key: fileName,
        ContentType: file.type,
        Body: file,
    };

    try {
        const command = new PutObjectCommand(params);
        const data = await s3Client.send(command);
        console.log("画像アップロード成功:", data);
        return `https://${params.Bucket}.s3.${process.env.NEXT_PUBLIC_REGION}.amazonaws.com/${fileName}`;
    } catch (error) {
        console.error("画像アップロードエラー:", error);
        return null;
    }
}

export function PhotoCreateForm({ tagId, existingPhotosCount, onSuccess, disabled }: PhotoCreateFormProps) {
    const [isOpen, setIsOpen] = useState(false);
    const [title, setTitle] = useState('');
    const [detailedTitle, setDetailedTitle] = useState('');
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [previewUrl, setPreviewUrl] = useState<string>('');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const fileInputRef = useRef<HTMLInputElement>(null);
    const { user } = useAuth();
    const { toast } = useToast();

    const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files[0]) {
            const file = e.target.files[0];
            if (!file.type.startsWith('image/')) {
                toast({
                    title: "エラー",
                    description: "画像ファイルを選択してください。",
                    variant: "destructive"
                });
                return;
            }

            setSelectedFile(file);
            const reader = new FileReader();
            reader.onloadend = () => {
                setPreviewUrl(reader.result as string);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!user || !selectedFile) return;

        try {
            setIsSubmitting(true);

            const imageUrl = await uploadToS3(selectedFile);
            if (!imageUrl) {
                throw new Error("画像のアップロードに失敗しました");
            }

            const img = new Image();
            img.src = URL.createObjectURL(selectedFile);
            img.onload = async () => {
                const imageSize = { width: img.width, height: img.height };

                await api.createPhoto({
                    title,
                    detailed_title: detailedTitle,
                    content: imageUrl,
                    height: imageSize.height,
                    width: imageSize.width,
                    order: existingPhotosCount + 1,
                    tag_id: Number(tagId)
                });

                toast({
                    title: "成功",
                    description: "新しい写真を追加しました。",
                });

                setTitle('');
                setDetailedTitle('');
                setSelectedFile(null);
                setPreviewUrl('');
                setIsOpen(false);
                onSuccess?.();
            };
        } catch (error) {
            toast({
                title: "エラー",
                description: "写真の追加に失敗しました。",
                variant: "destructive"
            });
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={setIsOpen}>
            <DialogTrigger asChild>
                <Button variant="outline" className="flex items-center" disabled={disabled}>
                    写真を追加
                </Button>
            </DialogTrigger>
            <DialogContent className="bg-white">
                <DialogHeader>
                    <DialogTitle>新しい写真を追加</DialogTitle>
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
                    <div>
                        <label className="block text-sm font-medium text-gray-700">
                            画像
                        </label>
                        <Input
                            type="file"
                            accept="image/*"
                            onChange={handleFileSelect}
                            ref={fileInputRef}
                            required
                            className="mt-1"
                        />
                        {previewUrl && (
                            <div className="mt-2">
                                <img
                                    src={previewUrl}
                                    alt="Preview"
                                    className="max-w-full h-auto max-h-48 object-contain"
                                />
                            </div>
                        )}
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
                            disabled={isSubmitting || !selectedFile}
                        >
                            {isSubmitting ? '追加中...' : '追加'}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
}
