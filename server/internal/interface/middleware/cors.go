package middleware

import (
	"github.com/gin-gonic/gin"
)

// CORSミドルウェア
func CORS() gin.HandlerFunc {
	//allowedOrigin := os.Getenv("ALLOWED_ORIGIN") // 環境変数でオリジンを管理

	return func(c *gin.Context) {
		c.Header("Access-Control-Allow-Origin", "*") // すべてのオリジンを許可
		c.Header("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS")
		c.Header("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, X-Requested-With")
		c.Header("Access-Control-Allow-Credentials", "true")
		c.Header("Access-Control-Max-Age", "86400")

		//if c.Request.Method == http.MethodOptions {
		//	c.AbortWithStatus(http.StatusOK)
		//	return
		//}

		c.Next()
	}
}
