/*
Methods for determining JSON and numerical content.
 */

Object.assign(String.prototype, {
    containsJSON() {
        try {
            JSON.parse(this);
        } catch (e) {
            return false;
        }
        return true;
    }})

Object.assign(String.prototype, {
    containsNumber() {
        return /^-?\d+$/.test(this);
    }
})