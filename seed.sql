CREATE TABLE IF NOT EXISTS produtos (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    descricao TEXT,
    preco NUMERIC(10, 2) NOT NULL CHECK (preco >= 0),
    quantidade_estoque INT NOT NULL DEFAULT 0 CHECK (quantidade_estoque >= 0),
    data_cadastro TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

TRUNCATE TABLE produtos RESTART IDENTITY;

-- FAIXA 1: ESTOQUE CRITICO (<= 5 unidades) -> Alerta Vermelho
INSERT INTO produtos (nome, descricao, preco, quantidade_estoque, data_cadastro) VALUES
('Pasta Termica Prata 5g', 'Composto termico de alto desempenho para CPUs', 45.90, 2, CURRENT_TIMESTAMP),
('Cabo HDMI 2.1 Ultra HD 2m', 'Suporte a 4K 120Hz e 8K 60Hz com malha reforcada', 59.00, 3, CURRENT_TIMESTAMP),
('Mousepad Gamer Speed Extra Grande', 'Superficie de tecido 900x400mm bordas costuradas', 79.90, 5, CURRENT_TIMESTAMP);

-- FAIXA 2: ESTOQUE NORMAL (6 a 10 unidades) -> Indicador Verde
INSERT INTO produtos (nome, descricao, preco, quantidade_estoque, data_cadastro) VALUES
('Mouse Sem Fio Ergonomico', 'Sensor optico 2400 DPI conexao 2.4GHz USB', 89.90, 6, CURRENT_TIMESTAMP),
('Teclado Mecanico RGB Switch Blue', 'Layout ABNT2 com anti-ghosting em todas as teclas', 189.90, 8, CURRENT_TIMESTAMP),
('Hub USB-C 7 em 1 Aluminio', 'Saidas HDMI 4K, 3x USB 3.0, Leitor SD e PD 100W', 149.00, 10, CURRENT_TIMESTAMP);

-- FAIXA 3: ESTOQUE EXCEDENTE (> 10 unidades) -> Alerta Laranja
INSERT INTO produtos (nome, descricao, preco, quantidade_estoque, data_cadastro) VALUES
('Fonte ATX 500W 80 Plus Bronze', 'PFC Ativo bivolt automatico e cabos flat pretos', 269.90, 14, CURRENT_TIMESTAMP),
('Air Cooler CPU AG400', '4 heatpipes de cobre e fan de 120mm PWM silencioso', 139.90, 18, CURRENT_TIMESTAMP),
('Memoria RAM DDR4 8GB 3200MHz', 'Modulo de memoria desktop com dissipador termico', 129.90, 25, CURRENT_TIMESTAMP);
