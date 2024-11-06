package handler

import (
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/gorilla/sessions"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase"
	"github.com/zono0013/MyMuseGolangAPI/internal/usecase/input"
	"net/http"
)

type UserHandler struct {
	userUseCase usecase.UserUseCase
	store       *sessions.CookieStore
}

func NewUserHandler(userUseCase usecase.UserUseCase, store *sessions.CookieStore) *UserHandler {
	return &UserHandler{
		userUseCase: userUseCase,
		store:       store,
	}
}

func (h *UserHandler) HandleGoogleLogin(c *gin.Context) {
	url := h.userUseCase.GetAuthURL()
	c.Redirect(http.StatusTemporaryRedirect, url)
}

func (h *UserHandler) HandleGoogleCallback(c *gin.Context) {
	code := c.Query("code")
	state := c.Query("state")

	input := input.GoogleAuthCallback{
		Code:  code,
		State: state,
	}

	user, err := h.userUseCase.HandleGoogleCallback(c.Request.Context(), input)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	fmt.Println(user)

	// セッションの取得
	session, err := h.store.Get(c.Request, "session-name")
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Session error"})
		return
	}

	// セッションにユーザー情報を保存
	session.Values["user_id"] = user.ID
	session.Values["email"] = user.Email
	session.Values["name"] = user.Name
	session.Values["picture"] = user.Picture // ユーザーの画像URLも保存

	// セッションの設定を調整
	session.Options = &sessions.Options{
		Path:     "/",
		MaxAge:   60 * 60 * 24, // セッションの有効期限（適宜調整）
		HttpOnly: true,
		Secure:   false, // HTTPSが有効な環境で使用
		SameSite: http.SameSiteLaxMode,
	}

	fmt.Println("session")
	fmt.Println(session)

	if err := session.Save(c.Request, c.Writer); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to save session"})
		return
	}

	// フロントエンドのダッシュボードへリダイレクト
	c.Redirect(http.StatusFound, "http://localhost:3000/")
}

// 新しいエンドポイント: セッション検証
func (h *UserHandler) CheckSession(c *gin.Context) {
	fmt.Println("check session request")
	fmt.Println(c.Request)
	session, err := h.store.Get(c.Request, "session-name")
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Session error"})
		return
	}

	fmt.Printf("session: %+v\\n", session)

	if auth, ok := session.Values["user_id"].(string); !ok || auth == "" {
		c.JSON(http.StatusUnauthorized, gin.H{"error": "Unauthorized"})
		return
	}

	// ユーザー情報をレスポンスとして返す
	response := map[string]interface{}{
		"user_id": session.Values["user_id"],
		"email":   session.Values["email"],
		"name":    session.Values["name"],
		"picture": session.Values["picture"], // ユーザーの画像URLを含む
		"status":  "authenticated",
	}

	fmt.Println(response)

	c.JSON(http.StatusOK, response)
}

func (h *UserHandler) HandleLogout(c *gin.Context) {
	fmt.Println("logoutHandler")
	// セッションを取得
	session, err := h.store.Get(c.Request, "session-name")
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Session error"})
		return
	}

	// セッションの値をクリア
	session.Values = make(map[interface{}]interface{})

	// セッションの有効期限を0に設定して、すぐに無効化
	session.Options.MaxAge = -1

	// セッションを保存（これで削除が確定する）
	if err := session.Save(c.Request, c.Writer); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to clear session"})
		return
	}

	fmt.Printf("logout: %v\n", session.Values) // フォーマットを修正

}

func (h *UserHandler) GetAll(c *gin.Context) {
	// コンテキストを取得
	ctx := c.Request.Context()

	// UseCase を使って全ユーザー情報を取得
	allUsersOutput := h.userUseCase.GetAll(ctx)

	// レスポンスとしてユーザー情報を JSON 形式で返す
	c.JSON(200, allUsersOutput)
}
