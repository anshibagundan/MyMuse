package output

type UserTagsPhotosResponse struct {
	Tags []TagsPhotosResponse `json:"tags"`
}

type TagsPhotosResponse struct {
	ID       uint          `json:"ID"`
	Name     string        `json:"name"`
	RoomType string        `json:"roomType"`
	Photos   []PhotoOutput `json:"photos"`
}
