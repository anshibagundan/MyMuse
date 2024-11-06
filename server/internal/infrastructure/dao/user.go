package dao

import (
	"context"
	"errors"
	"fmt"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
	"gorm.io/gorm"
)

type userRepository struct {
	db *gorm.DB
}

func NewUserRepository(db *gorm.DB) *userRepository {
	return &userRepository{db: db}
}

func (r *userRepository) FindByID(ctx context.Context, userID string) (*model.User, error) {
	var user model.User

	fmt.Println(userID)

	// データベース操作
	result := r.db.WithContext(ctx).Where("id = ?", userID).First(&user)
	if result.Error != nil {
		// エラーが "Record Not Found" の場合には nil, nil を返す
		if result.Error == gorm.ErrRecordNotFound {
			return nil, errors.New("ユーザーが見つかりませんでした")
		}
		// それ以外のエラーはそのまま返す
		return nil, result.Error
	}

	return &user, nil
}

func (r *userRepository) Create(ctx context.Context, user *model.User) error {
	return r.db.WithContext(ctx).Create(user).Error
}

func (r *userRepository) Update(ctx context.Context, user *model.User) error {
	return r.db.WithContext(ctx).Save(user).Error
}

func (r *userRepository) GetTagsByUserID(ctx context.Context, userID string) ([]model.Tag, error) {
	var tags []model.Tag
	if err := r.db.WithContext(ctx).Where("user_id = ?", userID).Order("`order` ASC").Find(&tags).Error; err != nil {
		return nil, err
	}
	return tags, nil

}

func (r *userRepository) GetAll(ctx context.Context) ([]model.User, error) {
	var users []model.User

	// Example database query (assuming you are using something like GORM)
	if err := r.db.WithContext(ctx).Find(&users).Error; err != nil {
		return nil, err
	}

	return users, nil
}
