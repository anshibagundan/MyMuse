// internal/interface/middleware/auth.go

package middleware

import (
	"github.com/gin-gonic/gin"
	"github.com/gorilla/sessions"
	"net/http"
)

type AuthMiddleware struct {
	store *sessions.CookieStore
}

func NewAuthMiddleware(store *sessions.CookieStore) *AuthMiddleware {
	return &AuthMiddleware{
		store: store,
	}
}

func (m *AuthMiddleware) Authenticate(c *gin.Context) {
	session, err := m.store.Get(c.Request, "session-name")
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Session error"})
		c.Abort()
		return
	}

	userID, ok := session.Values["user_id"].(string)
	if !ok || userID == "" {
		c.JSON(http.StatusUnauthorized, gin.H{"error": "Authentication required"})
		c.Abort()
		return
	}

	c.Set("user_id", userID)
	c.Next()
}
