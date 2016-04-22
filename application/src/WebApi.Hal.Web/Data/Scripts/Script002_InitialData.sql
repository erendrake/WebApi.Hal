INSERT INTO Brewery ([Name])
VALUES
	('Little Creatures'),
	('BrewDog '),
	('Nøgne Ø'),
	('8 Wired Brewing Co.'),
	('Sierra Nevada Brewing Co.'),
	('Rogue Ales')
GO

INSERT INTO BeerStyle ([Name])
VALUES
	('American Double / Imperial IPA'),
	('American Amber / Red Ale'),
	('Winter Warmer'),
	('American Pale Ale'),
	('English Pale Ale'),
	('English Porter'),
	('Tripel'),
	('American IPA'),
	('Maibock / Helles Bock')
GO

INSERT INTO Beer ([Name],StyleId, BreweryId)
VALUES
	('Hopwired IPA', 1, 4),
	('Tall Poppy', 2, 4),
	('Day Of The Long Shadow', 3, 1),
	('Little Creatures Pale Ale', 4, 1),
	('Rogers', 5, 1),
	('Hardcore IPA', 1, 2),
	('God Jul', 6, 3),
	('Tiger Tripel', 7, 3),
	('Nøgne Ø India Pale Ale', 8, 3),
	('Sierra Nevada Celebration Ale', 8, 5),
	('Sierra Nevada Pale Ale', 4, 5),
	('Red Fox Amber Ale', 2, 6),
	('Dead Guy Ale', 9, 6),
	('5 A.M. Saint', 2, 2)
GO
