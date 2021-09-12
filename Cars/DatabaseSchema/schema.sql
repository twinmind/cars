CREATE TABLE brands (
    id integer NOT NULL,
    name text NULL,
    country text NULL,
    CONSTRAINT pk_brands PRIMARY KEY (id)
);


CREATE TABLE models (
    id integer NOT NULL,
    name text NULL,
    type text NULL,
    doors_count integer NOT NULL,
    brand_id integer NOT NULL,
    CONSTRAINT pk_models PRIMARY KEY (id),
    CONSTRAINT fk_models_brands_brand_id FOREIGN KEY (brand_id) REFERENCES brands (id) ON DELETE CASCADE
);


CREATE INDEX ix_models_brand_id ON models (brand_id);


