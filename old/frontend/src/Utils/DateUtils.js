import moment from "moment";

function formatDate(d) {
    if(d === 'never')
        return d

    return moment(d).format('L LTS')
}

function updateTimestampIfChanged(oldRec, newRec) {
    const s1 = JSON.stringify(oldRec)
    const s2 = JSON.stringify(newRec)
    if(s1 !== s2)
        newRec.thumbnail.updated = moment()

}

export { formatDate, updateTimestampIfChanged }