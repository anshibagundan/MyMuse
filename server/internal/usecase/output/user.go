package output

type UserOutput struct {
	ID      string
	Email   string
	Name    string
	Picture string
}

type AllUserOutput struct {
	Users []UserOutput
}
