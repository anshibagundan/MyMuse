package main

import (
	"fmt"
	"github.com/rs/cors"
	"github.com/zono0013/MyMuseGolangAPI/config"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/dao"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/persistence/mysql"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/router"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/handler"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
	"log"
	"net/http"
	"os"
)

func main() {
	cfg := config.Load()
	fmt.Println(cfg)

	db := mysql.NewDBConnection()
	fmt.Println("Database connection success")

	userRepository := dao.NewUserRepository(db)
	userUseCase := usecase.NewUserUseCase(userRepository)
	userHandler := handler.NewUserHandler(userUseCase)

	tagRepository := dao.NewTagRepository(db)
	tagUsecase := usecase.NewTagUsecase(tagRepository, userRepository)
	tagHandler := handler.NewTagHandler(tagUsecase)

	photoRepository := dao.NewPhotoRepository(db)
	photousecase := usecase.NewPhotoUsecase(photoRepository)
	photoHandler := handler.NewPhotoHandler(photousecase)

	tagsPhotosUsecase := usecase.NewTagsPhotosUseCase(userRepository, tagRepository)
	tagsPhotosHandler := handler.NewTagsPhotosHnadler(tagsPhotosUsecase)

	fmt.Println("User handler success")

	router := router.NewRouter(userHandler, tagHandler, photoHandler, tagsPhotosHandler)
	fmt.Println("Router success")

	port := os.Getenv("PORT")
	if port == "" {
		port = "8080" // Default for local development
	}

	handler := cors.AllowAll().Handler(router)

	log.Fatal(http.ListenAndServe(":"+port, handler))
}
