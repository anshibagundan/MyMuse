package config

import (
	"github.com/joho/godotenv"
	"os"
)

type Config struct {
	Server struct {
		Port int
	}
	DB struct {
		Host     string
		Port     string
		User     string
		Password string
		Database string
	}
	Google struct {
		ClientID     string
		ClientSecret string
		RedirectURL  string
	}
	Session struct {
		Key    string
		MaxAge int
	}
}

func Load() *Config {
	// 設定の読み込みロジック
	err := godotenv.Load()
	if err != nil {
		return nil
	}

	return &Config{
		Server: struct{ Port int }{Port: 8080},
		DB: struct {
			Host     string
			Port     string
			User     string
			Password string
			Database string
		}{Host: os.Getenv("DB_HOST"), Port: os.Getenv("DB_PORT"), User: os.Getenv("DB_USER"), Password: os.Getenv("DB_PASSWORD"), Database: os.Getenv("DB_NAME")},
		Google: struct {
			ClientID     string
			ClientSecret string
			RedirectURL  string
		}{ClientID: os.Getenv("GOOGLE_CLIENT_ID"), ClientSecret: os.Getenv("GOOGLE_CLIENT_SECRET"), RedirectURL: os.Getenv("GOOGLE_CALLBACK_URL")},
		Session: struct {
			Key    string
			MaxAge int
		}{Key: os.Getenv(""), MaxAge: 100},
	}
}

func GetEnvWithDefault(name string, def string) string {
	env := os.Getenv(name)
	if len(env) != 0 {
		return env
	}
	return def
}
