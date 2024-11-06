CREATE TABLE users (
                       id VARCHAR(255) PRIMARY KEY,
                       email VARCHAR(255) UNIQUE NOT NULL,
                       name VARCHAR(255) NOT NULL,
                       picture TEXT,
                       access_token TEXT,
                       refresh_token TEXT,
                       verified_email BOOLEAN,
                       created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                       updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
