export const formatarNumero = (valor) => {
    if (isNaN(valor) || valor == '')
        return valor;
    else
        return roundTo(valor, 2).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' }).replace('R$', '').replace(/ /g, '');
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


export const formatarNumeroUS = (valor) => {
    var result = valor.replace(/[^0-9]/g, '');
    if (/[,\.]\d{2}$/.test(valor)) {
        result = result.replace(/(\d{2})$/, '.$1');
    }
    return result;
}