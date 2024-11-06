package repository

import (
	"context"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
)

type PhotoRepository interface {
	//GetAll(ctx context.Context)
	Create(ctx context.Context, photo *model.Photo) error
	Update(ctx context.Context, photo *model.Photo) error
	Delete(ctx context.Context, id string) error
	UpdateOrder(ctx context.Context, photo *model.Photo, id string) error
}
