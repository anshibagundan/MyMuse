package model

type User struct {
	ID       string `gorm:"primaryKey;size:255" json:"id"`
	Email    string `gorm:"size:255;unique;not null" json:"email"`
	Password []byte `gorm:"type:varchar(255);not null"`
}
