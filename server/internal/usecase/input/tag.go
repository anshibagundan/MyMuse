package input

type CreateTagInput struct {
	Name     string `json:"name"`
	RoomType string `json:"roomType"`
	UserID   string `json:"user_id"`
	Order    int    `json:"order"`
}

type UpdateTagInput struct {
	ID       uint
	Name     string
	RoomType string
	UserID   string
	Order    int
}

type TagOrder struct {
	ID    uint `json:"id"`
	Order int  `json:"order"`
}

type TagOrderList []PhotoOrder
