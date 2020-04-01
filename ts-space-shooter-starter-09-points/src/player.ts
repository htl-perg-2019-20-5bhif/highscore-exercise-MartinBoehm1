export class Player{
    name: string;
    score: number;
    constructor(name: string, score: number) {
        this.name = name;
        this.score = score;
    }
    comparer(p1: Player,p2: Player){
        return p2.score-p1.score;
    }
}