package usecase

import (
	"context"
	"fmt"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/repository"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/input"
)

type photoUsecase struct {
	photoRepo repository.PhotoRepository
}

func NewPhotoUsecase(photoRepo repository.PhotoRepository) IPhotoUsecase {
	return &photoUsecase{
		photoRepo: photoRepo,
	}
}

type IPhotoUsecase interface {
	Create(ctx context.Context, input input.CreatePhotoInput) error
	Update(ctx context.Context, photoInput input.UpdatePhotoInput) error
	Delete(ctx context.Context, id string) error
	UpdateOrder(ctx context.Context, input input.PhotoOrderList, id string) error
}

func (u *photoUsecase) Create(ctx context.Context, input input.CreatePhotoInput) error {
	var photo *model.Photo
	photo = &model.Photo{
		Title:      input.Title,
		Detailed:   input.DetailedTitle,
		Content:    input.Content,
		Height:     input.Height,
		Width:      input.Width,
		TagID:      input.TagID,
		PhotoOrder: input.Order,
	}

	fmt.Println(photo)
	err := u.photoRepo.Create(ctx, photo)
	if err != nil {
		return err
	}
	return nil
}

func (u *photoUsecase) Update(ctx context.Context, input input.UpdatePhotoInput) error {
	var photo *model.Photo
	photo = &model.Photo{
		ID:       input.ID,
		Title:    input.Title,
		Detailed: input.DetailedTitle,
	}
	err := u.photoRepo.Update(ctx, photo)
	if err != nil {
		return err
	}
	return nil
}

func (u *photoUsecase) Delete(ctx context.Context, id string) error {
	return u.photoRepo.Delete(ctx, id)
}

func (u *photoUsecase) UpdateOrder(ctx context.Context, photos input.PhotoOrderList, id string) error {
	for _, p := range photos {
		var photo *model.Photo
		photo = &model.Photo{
			ID:         p.ID,
			PhotoOrder: p.Order,
		}
		err := u.photoRepo.UpdateOrder(ctx, photo, id)
		if err != nil {
			return err
		}
	}
	return nil
}
