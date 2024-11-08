package usecase

import (
	"context"
	"fmt"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"
	"golang.org/x/crypto/bcrypt"

	"github.com/zono0013/MyMuseGolangAPI/internal/domain/repository"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/input"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/output"
)

type UserUseCase interface {
	GetAll(ctx context.Context) output.AllUserOutput
	Login(ctx context.Context, user input.User) (*output.UserOutput, error)
}

type userUseCase struct {
	userRepo repository.UserRepository
}

func NewUserUseCase(userRepo repository.UserRepository) UserUseCase {
	return &userUseCase{
		userRepo: userRepo,
	}
}

func (u *userUseCase) GetAll(ctx context.Context) output.AllUserOutput {
	// userRepo から全ユーザーを取得
	users, err := u.userRepo.GetAll(ctx)
	if err != nil {
		// エラー時の処理
		return output.AllUserOutput{
			Users: []output.UserOutput{}, // 空のユーザーリストを返す
		}
	}

	// model.User を output.UserOutput に変換
	var userOutputs []output.UserOutput
	for _, user := range users {
		userOutput := output.UserOutput{
			ID:    user.ID,
			Email: user.Email,
		}
		userOutputs = append(userOutputs, userOutput)
	}

	// 変換したユーザーリストを AllUserOutput にセットして返す
	return output.AllUserOutput{
		Users: userOutputs,
	}
}

func (u *userUseCase) Login(ctx context.Context, input input.User) (*output.UserOutput, error) {
	var user *model.User

	// パスワードをハッシュ化
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(input.Password), bcrypt.DefaultCost)
	if err != nil {
		return nil, fmt.Errorf("failed to hash password: %v", err)
	}

	user, err = u.userRepo.FindByEmail(ctx, input.Email)
	if err != nil {
		// 新規ユーザーの作成
		user = &model.User{
			Email:    input.Email,
			Password: hashedPassword,
		}
		if err := u.userRepo.Create(ctx, user); err != nil {
			return nil, err
		}
		user, err = u.userRepo.FindByEmail(ctx, input.Email)
	}

	return &output.UserOutput{
		ID:    user.ID,
		Email: user.Email,
	}, nil
}
