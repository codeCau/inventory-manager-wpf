TRUNCATE TABLE produtos RESTART IDENTITY;

INSERT INTO produtos (nome, descricao, preco, quantidade_estoque, data_cadastro) VALUES
-- ESTOQUE CRÍTICO (0 a 5 un. -> Destacam em Vermelho para chamar atenção na demonstração)
('Processador AMD Ryzen 7 5700X', '8 núcleos 16 threads 4.6GHz AM4 sem cooler', 1189.90, 2, CURRENT_TIMESTAMP),
('Placa de Vídeo RTX 4060 8GB GDDR6', 'Dual Fan DLSS 3 Ray Tracing PCI-E 4.0', 2149.00, 1, CURRENT_TIMESTAMP),
('SSD NVMe M.2 1TB PCIe 4.0 5000MB/s', 'Leitura ultrarrápida para boot e jogos pesados', 439.90, 3, CURRENT_TIMESTAMP),
('Water Cooler 240mm ARGB Preto', 'Bomba cerâmica com duas ventoinhas de alto fluxo', 369.90, 4, CURRENT_TIMESTAMP),
('Pasta Térmica Alta Condutividade 4g', 'Condutividade 12.8 W/mK com espátula inclusa', 49.90, 2, CURRENT_TIMESTAMP),
('Cabo Extensor Sleeved Full Black', 'Kit cabos modulares ATX 24p e PCIe 8p', 129.90, 5, CURRENT_TIMESTAMP),
('Headset Gamer 7.1 Surround USB', 'Drivers de 53mm com microfone condensador destacável', 279.00, 3, CURRENT_TIMESTAMP),
('Monitor Gamer 24" IPS 165Hz 1ms', 'Freesync Premium com ajuste de altura e inclinação', 899.90, 2, CURRENT_TIMESTAMP),
('Suporte Articulado a Gás para 2 Monitores', 'Fixação em morsa padrão VESA 75x75 e 100x100', 259.90, 1, CURRENT_TIMESTAMP),
('Microfone USB Condensador Podcast', 'Padrão polar cardióide com tripé e pop filter', 199.90, 4, CURRENT_TIMESTAMP),

-- ESTOQUE NORMAL (6 a 10 un. -> Faixa operacional verde)
('Mouse Gamer Óptico 16000 DPI', 'Sensor PixArt 3389 switches ópticos 70M cliques', 219.90, 8, CURRENT_TIMESTAMP),
('Teclado Mecânico ABNT2 RGB Switch Brown', 'Keycaps doubleshot com apoio magnético de pulso', 349.90, 6, CURRENT_TIMESTAMP),
('Fonte ATX 650W 80 Plus Gold Modular', 'PFC Ativo com proteções OVP/UVP/SCP', 489.90, 9, CURRENT_TIMESTAMP),
('Gabinete Gamer Mid Tower Vidro Lateral', 'Painel mesh com 3 fans frontais pré-instaladas', 319.90, 7, CURRENT_TIMESTAMP),
('Webcam Full HD 1080p 60FPS com Autofoco', 'Microfone duplo estéreo e obturador de privacidade', 289.90, 10, CURRENT_TIMESTAMP),
('Cadeira Ergonômica Mesh com Braço 3D', 'Apoio lombar ajustável e mecanismo relax', 799.00, 6, CURRENT_TIMESTAMP),
('Roteador Wi-Fi 6 Gigabit Dual Band', 'Velocidade combinada até 3000Mbps com 4 antenas', 359.90, 8, CURRENT_TIMESTAMP),
('Placa Mãe B550M Micro-ATX AM4', 'Dissipadores reforçados e 2 slots M.2 PCIe 4.0', 689.90, 7, CURRENT_TIMESTAMP),
('Controle Sem Fio Bluetooth PC/Xbox', 'Gatilhos de impulso e textura aderente', 389.00, 9, CURRENT_TIMESTAMP),
('Hub USB-C 8 em 1 Gigabit Ethernet', 'Pass-through PD 100W, HDMI 4K e leitor de cartões', 179.90, 10, CURRENT_TIMESTAMP),

-- ESTOQUE EXCEDENTE / CHEIO (> 10 un. -> Alerta em Laranja)
('Memória RAM DDR4 8GB 3200MHz', 'Módulo CL16 com dissipador de alumínio cinza', 139.90, 32, CURRENT_TIMESTAMP),
('Cabo de Rede Cat6 UTP 3m Blindado', 'Conectores RJ45 banhados a ouro 50u', 19.90, 45, CURRENT_TIMESTAMP),
('Mousepad Speed Extra Grande 900x400mm', 'Bordas costuradas e base de borracha antiderrapante', 69.90, 24, CURRENT_TIMESTAMP),
('Cabo HDMI 2.1 Ultra HD 8K 2 metros', 'Largura de banda 48Gbps malha trançada reforçada', 59.90, 18, CURRENT_TIMESTAMP),
('Kit 3 Ventoinhas 120mm ARGB com Controladora', 'Rolamento hidráulico e amortecedores de borracha', 119.90, 15, CURRENT_TIMESTAMP),
('Pendrive 64GB USB 3.2 Retrátil', 'Velocidade de transferência até 100MB/s', 39.90, 40, CURRENT_TIMESTAMP),
('Adaptador Bluetooth 5.3 Nano USB', 'Alcance de até 20 metros sem delay de áudio', 35.00, 28, CURRENT_TIMESTAMP),
('Filtro de Linha DPS 8 Tomadas Bivolt', 'Chave disjuntora e proteção contra surtos elétricos', 89.90, 16, CURRENT_TIMESTAMP),
('Organizador de Cabos Espiral 2 metros', 'Acompanha presilhas de velcro reutilizáveis', 22.90, 35, CURRENT_TIMESTAMP),
('Suporte Vertical para Headset Alumínio', 'Base emborrachada pesada com acabamento fosco', 54.90, 20, CURRENT_TIMESTAMP);
