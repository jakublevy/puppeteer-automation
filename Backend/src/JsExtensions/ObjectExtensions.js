/*
C++ collections functionality
 */
Object.assign(Object.prototype, {
    empty() {
        return Object.keys(this).length === 0
    }})