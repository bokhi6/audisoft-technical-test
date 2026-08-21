export function dividirNombreCompleto(nombreCompleto: string): { nombres: string; apellidos: string } {
  const partes = nombreCompleto.trim().split(/\s+/).filter(Boolean);
  if (partes.length <= 1) {
    return { nombres: partes[0] ?? '', apellidos: '' };
  }
  const apellidos = partes.pop()!;
  return { nombres: partes.join(' '), apellidos };
}

export function unirNombreCompleto(nombres: string, apellidos: string): string {
  return `${nombres.trim()} ${apellidos.trim()}`.trim();
}
