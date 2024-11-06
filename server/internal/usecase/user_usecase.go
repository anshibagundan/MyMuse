package usecase

import (
	"context"
	"encoding/json"
	"github.com/zono0013/MyMuseGolangAPI/internal/domain/model"

	"github.com/zono0013/MyMuseGolangAPI/internal/domain/repository"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/persistence/oauth2"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/input"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/output"
)

type UserUseCase interface {
	HandleGoogleCallback(ctx context.Context, input input.GoogleAuthCallback) (*output.UserOutput, error)
	GetAuthURL() string
	GetAll(ctx context.Context) output.AllUserOutput
}

type userUseCase struct {
	userRepo    repository.UserRepository
	oauthClient *oauth2.OAuth2Client
}

func NewUserUseCase(userRepo repository.UserRepository, oauthClient *oauth2.OAuth2Client) UserUseCase {
	return &userUseCase{
		userRepo:    userRepo,
		oauthClient: oauthClient,
	}
}

func (u *userUseCase) HandleGoogleCallback(ctx context.Context, input input.GoogleAuthCallback) (*output.UserOutput, error) {
	// トークンの取得
	token, err := u.oauthClient.Exchange(ctx, input.Code)
	if err != nil {
		return nil, err
	}

	// Googleユーザー情報の取得
	client := u.oauthClient.Client(ctx, token)
	resp, err := client.Get("https://www.googleapis.com/oauth2/v2/userinfo")
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	var googleUser struct {
		ID            string `json:"id"`
		Email         string `json:"email"`
		VerifiedEmail bool   `json:"verified_email"`
		Name          string `json:"name"`
		Picture       string `json:"picture"`
	}

	if err := json.NewDecoder(resp.Body).Decode(&googleUser); err != nil {
		return nil, err
	}

	// ユーザーの検索または作成
	user, err := u.userRepo.FindByID(ctx, googleUser.ID)
	if err != nil {

		// 新規ユーザーの作成
		user = &model.User{
			ID:            googleUser.ID,
			Email:         googleUser.Email,
			Name:          googleUser.Name,
			Picture:       googleUser.Picture,
			AccessToken:   token.AccessToken,
			RefreshToken:  token.RefreshToken,
			VerifiedEmail: googleUser.VerifiedEmail,
		}

		if err := u.userRepo.Create(ctx, user); err != nil {
			return nil, err
		}
	} else {
		// 既存ユーザーの更新
		user.AccessToken = token.AccessToken
		user.RefreshToken = token.RefreshToken
		if err := u.userRepo.Update(ctx, user); err != nil {
			return nil, err
		}
	}

	return &output.UserOutput{
		ID:      user.ID,
		Email:   user.Email,
		Name:    user.Name,
		Picture: user.Picture,
	}, nil
}

func (u *userUseCase) GetAuthURL() string {
	return u.oauthClient.GetAuthURL("state")
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
			ID:      user.ID,
			Email:   user.Email,
			Name:    user.Name,
			Picture: user.Picture,
		}
		userOutputs = append(userOutputs, userOutput)
	}

	// 変換したユーザーリストを AllUserOutput にセットして返す
	return output.AllUserOutput{
		Users: userOutputs,
	}
}
