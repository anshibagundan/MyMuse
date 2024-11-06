package repository

import (
	"context"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
)

type UserRepository interface {
	FindByID(ctx context.Context, userID string) (*model.User, error)
	Create(ctx context.Context, user *model.User) error
	Update(ctx context.Context, user *model.User) error
	GetTagsByUserID(ctx context.Context, userID string) ([]model.Tag, error)
	GetAll(ctx context.Context) ([]model.User, error)
}
