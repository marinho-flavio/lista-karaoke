import pdfplumber
import csv
import os

pdf_path = "/opt/Empresas/ses/lista-karaoke/docs/Catálogo .pdf"
csv_path = "/opt/Empresas/ses/lista-karaoke/docs/musicas.csv"

print(f"Iniciando conversão de: {pdf_path}")

with pdfplumber.open(pdf_path) as pdf:
    with open(csv_path, 'w', newline='', encoding='utf-8') as csvfile:
        writer = csv.writer(csvfile)
        # Cabeçalho do CSV
        writer.writerow(["Cantor", "Codigo", "Titulo", "InicioLetra"])
        
        total_pages = len(pdf.pages)
        for i, page in enumerate(pdf.pages):
            # Extrair tabela da página
            table = page.extract_table()
            if table:
                for row in table:
                    # Pular o cabeçalho da tabela que se repete em cada página
                    if row[0] == "CANTOR" or row[1] == "COD.":
                        continue
                    # Limpar dados e escrever no CSV
                    # row[0]=Cantor, row[1]=Cod, row[2]=Titulo, row[3]=Inicio
                    cleaned_row = [str(item).replace('\n', ' ').strip() if item else "" for item in row]
                    if any(cleaned_row): # Evitar linhas vazias
                        writer.writerow(cleaned_row)
            
            if (i + 1) % 50 == 0:
                print(f"Processado: {i + 1}/{total_pages} páginas...")

print(f"Sucesso! Arquivo gerado em: {csv_path}")
