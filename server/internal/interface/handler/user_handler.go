package handler

import (
	"github.com/gin-gonic/gin"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/request"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
	"net/http"
)

type UserHandler struct {
	userUseCase usecase.UserUseCase
}

func NewUserHandler(userUseCase usecase.UserUseCase) *UserHandler {
	return &UserHandler{
		userUseCase: userUseCase,
	}
}

func (h *UserHandler) HandleLogin(ctx *gin.Context) {
	request := request.CreateUserDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"error": err.Error()})
		return
	}

	user, err := h.userUseCase.Login(ctx, request)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(http.StatusOK, gin.H{"message": "login in successful", "user": user})
}

func (h *UserHandler) GetAll(c *gin.Context) {
	// コンテキストを取得
	ctx := c.Request.Context()

	// UseCase を使って全ユーザー情報を取得
	allUsersOutput := h.userUseCase.GetAll(ctx)

	// レスポンスとしてユーザー情報を JSON 形式で返す
	c.JSON(200, allUsersOutput)
}
