package model

import "time"

type Tag struct {
	ID        uint      `gorm:"primaryKey;autoIncrement" json:"id"`
	Name      string    `gorm:"size:100;not null" json:"name"`
	UserID    string    `gorm:"size:255;not null" json:"user_id"` // usersテーブルへの外部キー
	Order     int       `gorm:"not null" json:"order"`            // タグの順番
	RoomType  string    `gorm:"not null" json:"roomType"`
	CreatedAt time.Time `gorm:"autoCreateTime" json:"created_at"`
	UpdatedAt time.Time `gorm:"autoUpdateTime" json:"updated_at"`
}

func NewCreateTag(
	name string,
	roomType string,
	userID string,
	order int,
) *Tag {
	return &Tag{
		Name:     name,
		RoomType: roomType,
		UserID:   userID,
		Order:    order,
	}
}
