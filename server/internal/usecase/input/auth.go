package input

type GoogleAuthCallback struct {
	Code  string
	State string
}

type User struct {
	Email    string
	Password string
}
