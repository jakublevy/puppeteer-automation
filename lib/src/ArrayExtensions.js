Object.assign(Array.prototype, {
    removeAt(index) {
        this.splice(index, 1)
    }})

Object.assign(Array.prototype, {
    //Returns: number of removed elements
    dropWhile(pred, offset) {
        let i = offset
        while(pred(this[i]))
            ++i

        if(i-offset > 0) {
            this.splice(offset, i - offset)
            return i - offset
        }
        return 0
    }
})

Object.assign(Array.prototype, {
    //Returns: flag indicating whether an Array object is empty
    empty() {
        return this.length === 0
    }
})

Object.assign(Array.prototype, {
    insertAt(index, item) {
        this.splice(index, 0, item);
    }
})

Object.assign(Array.prototype, {
    removeRange(start, end) {
        this.splice(start, end - start + 1)
    }
})

Object.assign(Array.prototype, {
    removeFrom(idx, numberOfElements) {
        this.splice(idx, numberOfElements)
    }
})