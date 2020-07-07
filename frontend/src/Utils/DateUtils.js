import moment from "moment";

function formatDate(d) {
    if(d === 'never')
        return d

    return moment(d).format('D.M.YYYY HH:MM:SS')
}

export { formatDate }