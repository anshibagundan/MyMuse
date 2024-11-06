package usecase

import (
	"context"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/repository"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/output"
)

type TagsPhotosUseCase interface {
	GetAll(ctx context.Context, userID string) (*output.UserTagsPhotosResponse, error)
}

type tagsPhotosUseCase struct {
	userRepo repository.UserRepository
	tagRepo  repository.TagRepository
}

func NewTagsPhotosUseCase(userRepo repository.UserRepository, tagRepo repository.TagRepository) TagsPhotosUseCase {
	return &tagsPhotosUseCase{
		userRepo: userRepo,
		tagRepo:  tagRepo,
	}
}

func (u *tagsPhotosUseCase) GetAll(ctx context.Context, userID string) (*output.UserTagsPhotosResponse, error) {
	// ユーザーIDに紐づくタグを取得
	tags, err := u.userRepo.GetTagsByUserID(ctx, userID)
	if err != nil {
		return nil, err
	}

	// レスポンス用のタグリスト
	var tagsResponse []output.TagsPhotosResponse

	// 各タグに対して写真を取得し、レスポンス用のデータに変換
	for _, tag := range tags {
		photos, err := u.tagRepo.GetPhotosByTagID(ctx, tag.ID)
		if err != nil {
			return nil, err
		}

		// PhotosをPhotoOutputの形式に変換
		var photosResponse []output.PhotoOutput
		for _, photo := range photos {
			photoOutput := output.PhotoOutput{
				ID:            photo.ID,
				Title:         photo.Title,
				DetailedTitle: photo.Detailed,
				Content:       photo.Content,
			}
			photosResponse = append(photosResponse, photoOutput)
		}

		// タグの情報をTagsPhotosResponse形式に変換
		tagResponse := output.TagsPhotosResponse{
			ID:       tag.ID,
			RoomType: tag.RoomType,
			Name:     tag.Name,
			Photos:   photosResponse,
		}

		// レスポンスのタグリストに追加
		tagsResponse = append(tagsResponse, tagResponse)
	}

	// 最終レスポンス用の構造体を作成して返す
	response := &output.UserTagsPhotosResponse{
		Tags: tagsResponse,
	}

	return response, nil
}
