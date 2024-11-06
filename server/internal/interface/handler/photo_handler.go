package handler

import (
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/request"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
)

type photo_handler struct {
	photoHandler usecase.IPhotoUsecase
}

type IPhotoHandler interface {
	Create(ctx *gin.Context)
	Update(ctx *gin.Context)
	Delete(ctx *gin.Context)
	UpdateOrder(ctx *gin.Context)
}

func NewPhotoHandler(photoUsecase usecase.IPhotoUsecase) IPhotoHandler {
	return &photo_handler{
		photoHandler: photoUsecase,
	}
}

func (h *photo_handler) Create(ctx *gin.Context) {
	request := request.CreatePhotoDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"error": err.Error()})
		return
	}

	err := h.photoHandler.Create(ctx, request)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(200, "ok")
}

func (h *photo_handler) Update(ctx *gin.Context) {
	request := request.UpdatePhotoDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"error": err.Error()})
		return
	}

	err := h.photoHandler.Update(ctx, request)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(200, "ok")
}

func (h *photo_handler) Delete(ctx *gin.Context) {
	id := ctx.Param("photo_id")
	fmt.Println(id)
	err := h.photoHandler.Delete(ctx, id)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
	}

	ctx.JSON(204, "ok")
}

func (h *photo_handler) UpdateOrder(ctx *gin.Context) {
	id := ctx.Param("tag_id")

	request := request.UpdatePhotoOrderDTO{}
	if err := ctx.ShouldBindJSON(&request); err != nil {
		ctx.JSON(401, gin.H{"error": err.Error()})
		return
	}

	err := h.photoHandler.UpdateOrder(ctx, request, id)
	if err != nil {
		ctx.JSON(500, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(200, "ok")

}
