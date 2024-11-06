package handler

import (
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
)

type tags_photos_handler struct {
	tagsPhotosUsecase usecase.TagsPhotosUseCase
}

func NewTagsPhotosHnadler(useCase usecase.TagsPhotosUseCase) ITagsPhotos {
	return &tags_photos_handler{
		tagsPhotosUsecase: useCase,
	}
}

type ITagsPhotos interface {
	GetAll(ctx *gin.Context)
}

func (h *tags_photos_handler) GetAll(c *gin.Context) {
	id := c.Param("user_id")
	fmt.Println(id)
	fmt.Println("id")
	output, err := h.tagsPhotosUsecase.GetAll(c, id)
	if err != nil {
		c.JSON(500, gin.H{"error": err.Error()})
	}

	c.JSON(200, output)
}
