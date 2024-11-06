package middleware

import (
	"context"
	"github.com/gorilla/sessions"
	"net/http"
)

type SessionMiddleware struct {
	store sessions.Store
}

func NewSessionMiddleware(store sessions.Store) *SessionMiddleware {
	return &SessionMiddleware{store: store}
}

func (m *SessionMiddleware) Handle(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		session, err := m.store.Get(r, "session-name")
		if err != nil {
			http.Error(w, "Session error", http.StatusInternalServerError)
			return
		}

		// コンテキストにセッションを追加
		ctx := context.WithValue(r.Context(), "session", session)
		next.ServeHTTP(w, r.WithContext(ctx))
	})
}
