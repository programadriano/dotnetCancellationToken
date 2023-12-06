
docker run --name db-local -e POSTGRES_PASSWORD=102030 -d -p 5432:5432 postgres


CREATE TABLE Tickers (
    Code VARCHAR(20) PRIMARY KEY,
    CompanyName VARCHAR(100) NOT NULL
);

CREATE TABLE Transactions (
    Id SERIAL PRIMARY KEY,
    TickerCode VARCHAR(20) REFERENCES Tickers(Code),
    Amount DECIMAL NOT NULL,
    Quantity BIGINT NOT NULL,
    Date TIMESTAMP NOT NULL
);

-- Inserir dados na tabela Tickers
INSERT INTO Tickers (Code, CompanyName) VALUES ('AAPL', 'Apple Inc');
INSERT INTO Tickers (Code, CompanyName) VALUES ('MSFT', 'Microsoft Corporation');
INSERT INTO Tickers (Code, CompanyName) VALUES ('GOOGL', 'Alphabet Inc');
INSERT INTO Tickers (Code, CompanyName) VALUES ('AMZN', 'Amazon.com Inc');

-- Inserir dados na tabela Transactions
INSERT INTO Transactions (TickerCode, Amount, Quantity, Date) VALUES ('AAPL', 1500.00, 100, '2023-11-01');
INSERT INTO Transactions (TickerCode, Amount, Quantity, Date) VALUES ('MSFT', 2200.50, 150, '2023-11-02');
INSERT INTO Transactions (TickerCode, Amount, Quantity, Date) VALUES ('GOOGL', 3500.75, 200, '2023-11-03');
INSERT INTO Transactions (TickerCode, Amount, Quantity, Date) VALUES ('AMZN', 5000.25, 80, '2023-11-04');


{
    "ticker": {
        "code": "AAPL",
        "companyName": "Apple Inc"
    },
    "amount": 1500.00,
    "quantity": 100,
    "date": "2023-11-01T00:00:00"
}
