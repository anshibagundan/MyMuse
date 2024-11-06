package dao

import (
	"context"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
	"gorm.io/gorm"
)

type photoRepository struct {
	db *gorm.DB
}

func NewPhotoRepository(db *gorm.DB) *photoRepository {
	return &photoRepository{db: db}
}

func (r photoRepository) Create(ctx context.Context, photo *model.Photo) error {
	return r.db.WithContext(ctx).Create(photo).Error
}

func (r photoRepository) Update(ctx context.Context, photo *model.Photo) error {
	return r.db.WithContext(ctx).
		Model(&model.Photo{ID: photo.ID}). // モデル全体を指定
		Where("id = ?", photo.ID).         // 特定のユーザーを指定
		Updates(photo).Error
}

func (r photoRepository) Delete(ctx context.Context, id string) error {
	if err := r.db.WithContext(ctx).Where("id = ?", id).Delete(&model.Photo{}).Error; err != nil {
		return err // エラーが発生した場合はそのまま返す
	}
	return nil
}

func (r photoRepository) UpdateOrder(ctx context.Context, photo *model.Photo, id string) error {
	return r.db.WithContext(ctx).
		Model(&model.Photo{ID: photo.ID}). // モデル全体を指定
		Where("id = ?", photo.ID).         // 特定のユーザーを指定
		Updates(photo).Error
}
