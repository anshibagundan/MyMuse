package dao

import (
	"context"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
	"gorm.io/gorm"
)

type tagRepository struct {
	db *gorm.DB
}

func NewTagRepository(db *gorm.DB) *tagRepository {
	return &tagRepository{db: db}
}

func (r tagRepository) Create(ctx context.Context, tag *model.Tag) error {
	return r.db.WithContext(ctx).Create(tag).Error
}

func (r tagRepository) Update(ctx context.Context, tag *model.Tag) error {
	return r.db.WithContext(ctx).
		Model(&model.Tag{ID: tag.ID}). // モデル全体を指定
		Where("id = ?", tag.ID).       // 特定のユーザーを指定
		Updates(tag).Error
}

func (r tagRepository) Delete(ctx context.Context, id string) error {
	if err := r.db.WithContext(ctx).Where("id = ?", id).Delete(&model.Tag{}).Error; err != nil {
		return err // エラーが発生した場合はそのまま返す
	}
	return nil
}

func (r tagRepository) GetPhotosByTagID(ctx context.Context, tagID uint) ([]model.Photo, error) {
	var photos []model.Photo
	if err := r.db.WithContext(ctx).Where("tag_id = ?", tagID).Order("`photo_order` ASC").Find(&photos).Error; err != nil {
		return nil, err
	}
	return photos, nil
}

func (r tagRepository) UpdateOrder(ctx context.Context, tag *model.Tag, id string) error {
	return r.db.WithContext(ctx).
		Model(&model.Tag{ID: tag.ID}). // モデル全体を指定
		Where("id = ?", tag.ID).       // 特定のユーザーを指定
		Updates(tag).Error
}
