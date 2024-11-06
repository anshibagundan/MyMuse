package main

import (
	"fmt"
	"github.com/gorilla/sessions"
	"github.com/zono0013/MyMuseGolangAPI/config"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/dao"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/persistence/mysql"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/persistence/oauth2"
	"github.com/zono0013/MyMuseGolangAPI/internal/infrastructure/router"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/handler"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
	"log"
	"net/http"
	"os"
)

func main() {
	cfg := config.Load()

	db := mysql.NewDBConnection()
	fmt.Println("Database connection success")
	oauthClient := oauth2.NewOAuth2Client(cfg.Google.ClientID, cfg.Google.ClientSecret, cfg.Google.RedirectURL)
	fmt.Println("OAuth2 Client success")
	store := sessions.NewCookieStore([]byte(os.Getenv("SESSION_KEY")))
	fmt.Println("Session store success")

	userRepository := dao.NewUserRepository(db)
	userUseCase := usecase.NewUserUseCase(userRepository, oauthClient)
	userHandler := handler.NewUserHandler(userUseCase, store)

	tagRepository := dao.NewTagRepository(db)
	tagUsecase := usecase.NewTagUsecase(tagRepository, userRepository)
	tagHandler := handler.NewTagHandler(tagUsecase)

	photoRepository := dao.NewPhotoRepository(db)
	photousecase := usecase.NewPhotoUsecase(photoRepository)
	photoHandler := handler.NewPhotoHandler(photousecase)

	tagsPhotosUsecase := usecase.NewTagsPhotosUseCase(userRepository, tagRepository)
	tagsPhotosHandler := handler.NewTagsPhotosHnadler(tagsPhotosUsecase)

	fmt.Println("User handler success")

	router := router.NewRouter(userHandler, tagHandler, photoHandler, tagsPhotosHandler, store)
	fmt.Println("Router success")

	log.Fatal(http.ListenAndServe(":8080", router))
}
