package model

import "time"

type Photo struct {
	ID         uint      `gorm:"primaryKey;autoIncrement" json:"id"`
	Title      string    `gorm:"size:40;not null" json:"title"`
	Detailed   string    `json:"detailed"`
	Content    string    `gorm:"type:text;not null" json:"content"`
	Height     int       `gorm:"not null" json:"height"`
	Width      int       `gorm:"not null" json:"width"`
	TagID      uint      `gorm:"not null" json:"tag_id"` // tagsテーブルへの外部キー
	PhotoOrder int       `gorm:"not null" json:"order"`  // タグ内での写真の順番
	CreatedAt  time.Time `gorm:"autoCreateTime" json:"created_at"`
	UpdatedAt  time.Time `gorm:"autoUpdateTime" json:"updated_at"`
}
