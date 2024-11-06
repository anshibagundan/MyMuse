package handler

import (
	"github.com/gin-gonic/gin"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/request"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
)

type tagHandler struct {
	tagUsecase usecase.ITagUsecase
}

type ITagHandler interface {
	Create(ctx *gin.Context)
	Update(ctx *gin.Context)
	Delete(ctx *gin.Context)
	UpdateOrder(ctx *gin.Context)
}

func NewTagHandler(tagUsecase usecase.ITagUsecase) ITagHandler {
	return &tagHandler{
		tagUsecase,
	}
}

func (h *tagHandler) Create(ctx *gin.Context) {
	// リクエストボディから JSON データをバインド
	request := request.CreateTagDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"error": err.Error()})
		return
	}

	// ユースケースの実行
	err := h.tagUsecase.Create(ctx, request)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(200, "ok")
}

func (h *tagHandler) Update(ctx *gin.Context) {
	request := request.UpdateTagDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"error": err.Error()})
		return
	}

	err := h.tagUsecase.Update(ctx, request)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(200, "ok")
}

func (h *tagHandler) Delete(ctx *gin.Context) {
	tagID := ctx.Param("tag_id")
	err := h.tagUsecase.Delete(ctx, tagID)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(204, "ok")
}

func (h *tagHandler) UpdateOrder(ctx *gin.Context) {
	userID := ctx.Param("user_id")
	request := request.UpdateTagOrderDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"error": err.Error()})
		return
	}
	err := h.tagUsecase.UpdateOrder(ctx, request, userID)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(200, "ok")
}
