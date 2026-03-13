--CREATE ANIMAL TABLE
CREATE TABLE Animal (
	"IdAnimal" INT PRIMARY KEY IDENTITY(1,1), 
	"Name" NVARCHAR(200), 
	"Description" NVARCHAR(200) NULL, 
	"CATEGORY" NVARCHAR(200),
	"AREA" NVARCHAR(200)
);

--INSERT 10 RANDOM ROWS TO ANIMAL TABLE
INSERT INTO Animal (Name, Description, CATEGORY, AREA)
VALUES 
    ('Leopard', 'Leopards are agile predators.', 'Mammal', 'Various'),
    ('Gorilla', 'Gorillas are the largest living primates.', 'Mammal', 'Africa'),
    ('Koala', NULL, 'Mammal', 'Australia'),
    ('Lizard', 'Lizards are reptiles with long bodies and tails.', 'Reptile', 'Various'),
    ('Ostrich', 'Ostriches are flightless birds with long necks.', 'Bird', 'Africa'),
    ('Shark', 'Sharks are cartilaginous fish known for their sharp teeth.', 'Fish', 'Ocean'),
    ('Polar Bear', 'Polar bears are marine mammals native to the Arctic.', 'Mammal', 'Arctic'),
    ('Gazelle', 'Gazelles are swift and graceful antelopes.', 'Mammal', 'Africa'),
    ('Hippopotamus', 'Hippos are large herbivorous mammals.', 'Mammal', 'Africa'),
    ('Zebra', NULL, 'Mammal', 'Africa');

    select * from animal