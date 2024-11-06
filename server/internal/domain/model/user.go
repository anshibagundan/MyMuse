package model

import "time"

type User struct {
	ID            string    `gorm:"primaryKey;size:255" json:"id"`
	Email         string    `gorm:"size:255;unique;not null" json:"email"`
	Name          string    `gorm:"size:255;not null" json:"name"`
	Picture       string    `gorm:"type:text" json:"picture"`
	AccessToken   string    `gorm:"type:text" json:"access_token"`
	RefreshToken  string    `gorm:"type:text" json:"refresh_token"`
	VerifiedEmail bool      `gorm:"not null" json:"verified_email"`
	CreatedAt     time.Time `gorm:"autoCreateTime" json:"created_at"`
	UpdatedAt     time.Time `gorm:"autoUpdateTime" json:"updated_at"`
	Tags          []Tag     `gorm:"foreignKey:UserID;constraint:OnDelete:CASCADE" json:"tags"` // タグのリレーション
}
