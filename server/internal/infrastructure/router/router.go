// internal/interface/router/router.go

package router

import (
	"github.com/gin-gonic/gin"
	"github.com/gorilla/sessions"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/handler"
	"github.com/zono0013/MyMuseGolangAPI/internal/interface/middleware"
)

func NewRouter(
	userHandler *handler.UserHandler,
	tagHandler handler.ITagHandler,
	photoHandler handler.IPhotoHandler,
	tagsPhotosHnadler handler.ITagsPhotos,
	store *sessions.CookieStore,
) *gin.Engine {
	router := gin.Default()

	// ミドルウェアの初期化
	//authMiddleware := middleware.NewAuthMiddleware(store)
	router.Use(middleware.CORS())

	router.GET("health", health)

	// 認証不要のルート
	auth := router.Group("/auth")
	{
		auth.GET("/google/login", userHandler.HandleGoogleLogin)
		auth.GET("/google/callback", userHandler.HandleGoogleCallback)
		auth.GET("/check-session", userHandler.CheckSession)
		auth.GET("/logout", userHandler.HandleLogout)
	}

	// 認証が必要なAPIルート
	api := router.Group("/api")
	//api.Use(authMiddleware.Authenticate)
	//{
	// Webアプリケーション用のエンドポイント
	web := api.Group("/web")
	{
		web.GET("/users", userHandler.GetAll)
		web.GET("/user/:user_id", tagsPhotosHnadler.GetAll)

		web.GET("/tag")
		web.POST("/tag", tagHandler.Create)
		web.PUT("/tag", tagHandler.Update)
		web.DELETE("/tag/:tag_id", tagHandler.Delete)
		web.PUT("/tag/order/:user_id", tagHandler.UpdateOrder)
		web.PUT("/tag/:tag_id/photos/order", photoHandler.UpdateOrder)

		web.GET("/photo")
		web.POST("/photo", photoHandler.Create)
		web.PUT("/photo", photoHandler.Update)
		web.DELETE("/photo/:photo_id", photoHandler.Delete)
	}

	// Unity用のエンドポイント
	unity := api.Group("/unity")
	{
		unity.GET("/login")
		unity.GET("/:user_id")
	}
	//}

	return router
}

func health(c *gin.Context) {
	health := "hogehoge"
	c.JSON(200, health)
}
