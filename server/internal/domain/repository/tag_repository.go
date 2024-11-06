package repository

import (
	"context"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
)

type TagRepository interface {
	//GetAll(ctx context.Context, email string) ([]*model.Tag, error)
	Create(ctx context.Context, tag *model.Tag) error
	Update(ctx context.Context, tag *model.Tag) error
	Delete(ctx context.Context, id string) error
	GetPhotosByTagID(ctx context.Context, tagID uint) ([]model.Photo, error)
	UpdateOrder(ctx context.Context, tag *model.Tag, id string) error
}
