package oauth2

import (
	"context"
	"fmt"
	"golang.org/x/oauth2"
	"golang.org/x/oauth2/google"
	"net/http"
)

type OAuth2Client struct {
	config *oauth2.Config
}

func NewOAuth2Client(clientID, clientSecret, redirectURL string) *OAuth2Client {
	config := &oauth2.Config{
		ClientID:     clientID,
		ClientSecret: clientSecret,
		RedirectURL:  redirectURL,
		Scopes: []string{
			"https://www.googleapis.com/auth/userinfo.email",
			"https://www.googleapis.com/auth/userinfo.profile",
		},
		Endpoint: google.Endpoint,
	}
	return &OAuth2Client{config: config}
}

func (c *OAuth2Client) GetAuthURL(state string) string {
	fmt.Println("GetAuthURL")
	return c.config.AuthCodeURL(state)
}

func (c *OAuth2Client) Exchange(ctx context.Context, code string) (*oauth2.Token, error) {
	return c.config.Exchange(ctx, code)
}

func (c *OAuth2Client) Client(ctx context.Context, token *oauth2.Token) *http.Client {
	return c.config.Client(ctx, token)
}
