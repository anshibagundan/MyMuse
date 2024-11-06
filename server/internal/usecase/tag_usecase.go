package usecase

import (
	"context"
	"fmt"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/repository"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/input"
)

type tagUsecase struct {
	tagRepo  repository.TagRepository
	userRepo repository.UserRepository
}

type ITagUsecase interface {
	Create(ctx context.Context, tag input.CreateTagInput) error
	Update(ctx context.Context, tag input.UpdateTagInput) error
	Delete(ctx context.Context, tag_id string) error
	UpdateOrder(ctx context.Context, tag input.TagOrderList, userID string) error
}

func NewTagUsecase(tagRepository repository.TagRepository, userRepository repository.UserRepository) ITagUsecase {
	return &tagUsecase{
		tagRepo:  tagRepository,
		userRepo: userRepository,
	}
}

func (t *tagUsecase) Create(ctx context.Context, input input.CreateTagInput) error {
	fmt.Printf("user: %s, tagname: %s, order: %d\n", input.UserID, input.Name, input.Order)

	_, err := t.userRepo.FindByID(ctx, input.UserID)
	if err != nil {
		fmt.Println("UserID not found")
		return err
	}

	return t.tagRepo.Create(ctx, model.NewCreateTag(input.Name, input.RoomType, input.UserID, input.Order))
}

func (t *tagUsecase) Update(ctx context.Context, input input.UpdateTagInput) error {
	var tag *model.Tag
	tag = &model.Tag{
		ID:       input.ID,
		Name:     input.Name,
		RoomType: input.RoomType,
		UserID:   input.UserID,
		Order:    input.Order,
	}
	return t.tagRepo.Update(ctx, tag)
}

func (t *tagUsecase) Delete(ctx context.Context, tag_id string) error {
	return t.tagRepo.Delete(ctx, tag_id)
}

func (t *tagUsecase) UpdateOrder(ctx context.Context, tags input.TagOrderList, id string) error {
	for _, d := range tags {
		var tag *model.Tag
		tag = &model.Tag{
			ID:    d.ID,
			Order: d.Order,
		}
		err := t.tagRepo.UpdateOrder(ctx, tag, id)
		if err != nil {
			return err
		}
	}
	return nil
}
