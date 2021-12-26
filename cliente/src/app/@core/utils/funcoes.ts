export const formatarNumero = (valor) => {
    if (isNaN(valor) || valor == '')
        return valor;
    else
        return roundTo(valor, 2).toLocaleString('pt-BR');
}

export const roundTo = (n, digits) => {
    if (digits === undefined) {
        digits = 0;
    }

    let multiplicator = Math.pow(10, digits);
    n = parseFloat((n * multiplicator).toFixed(11));
    var test = (Math.round(n) / multiplicator);
    return +(test.toFixed(digits));
}
