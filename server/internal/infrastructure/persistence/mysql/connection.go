package mysql

import (
	"fmt"
	"github.com/golang-migrate/migrate/v4"
	"time"

	"github.com/zono0013/MyMuseGolangAPI/config"

	migrate_mysql "github.com/golang-migrate/migrate/v4/database/mysql"
	_ "github.com/golang-migrate/migrate/v4/source/file"
	"github.com/rollbar/rollbar-go"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

func NewDBConnection() *gorm.DB {
	dsn := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s?charset=utf8mb4&parseTime=true&multiStatements=true",
		config.GetEnvWithDefault("DB_USER", "test_user"),
		config.GetEnvWithDefault("DB_PASSWORD", "test_password"),
		config.GetEnvWithDefault("DB_HOST", "db"),
		config.GetEnvWithDefault("DB_PORT", "3306"),
		config.GetEnvWithDefault("DB_NAME", "test_db"),
	)
	fmt.Println("Connecting to MySQL...")
	fmt.Println(dsn)
	db := connectDB(dsn)
	sqlDB, err := db.DB()
	if err != nil {
		rollbar.Error(err)
		panic(err)
	}
	dbDriver, err := migrate_mysql.WithInstance(sqlDB, &migrate_mysql.Config{})
	if err != nil {
		rollbar.Error(err)
		panic(err)
	}
	m, err := migrate.NewWithDatabaseInstance("file://migrations", "mysql", dbDriver)
	if err != nil {
		rollbar.Error(err)
		panic(err)
	}
	if err := m.Up(); err != nil && err != migrate.ErrNoChange {
		rollbar.Error(err)
		panic(err)
	}

	db.Logger = db.Logger.LogMode(logger.Info)

	return db
}

func connectDB(dsn string) *gorm.DB {
	db, err := openDBWithTimeLimit(dsn, 30)
	if err != nil {
		rollbar.Error(err)
		panic(err)
	}
	return db
}

func openDBWithTimeLimit(dsn string, count int) (*gorm.DB, error) {
	db, err := gorm.Open(mysql.Open(dsn), &gorm.Config{})
	if err != nil {
		if count == 0 {
			return nil, fmt.Errorf("retry count over")
		}
		time.Sleep(time.Second)
		count--
		return openDBWithTimeLimit(dsn, count)
	}
	return db, nil
}
